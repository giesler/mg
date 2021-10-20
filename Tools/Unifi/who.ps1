# Clients to Monitor
$uInterestingClients = @()
$uInterestingClients += "Mike - Galaxy S10"
$uInterestingClients += "Sara - Galaxy S10"
$uInterestingClients += "Neil - LG V60 ThinQ"
$uInterestingClients += "Shannon - Galaxy S9"

# Unifi Controller Login Base URI
$uController = 'https://192.168.4.2:8443' # e.g 'https://192.168.1.2:8443'
# Identifier of the site in UniFi. Set to Default to use the default site
$uSiteID = "default"

$uUsername = 'ms2n' # yourAdmin UserID
$uPassword = '2008InspectionReport!' # yourAdmin User Password
$uAuthBody = @{"username" = $uUsername; "password" = $uPassword }
$uHeaders = @{"Content-Type" = "application/json" }

# Allow connection with the Unifi Self Signed Cert
try {
    [System.Net.ServicePointManager]::CertificatePolicy = New-Object TrustAllCertsPolicy
}
catch {
    Write-Host "Error - $_"

    Add-Type @"
using System.Net;
using System.Security.Cryptography.X509Certificates;
public class TrustAllCertsPolicy : ICertificatePolicy {
    public bool CheckValidationResult(
        ServicePoint srvPoint, X509Certificate certificate,
        WebRequest request, int certificateProblem) {
        return true;
    }
}
"@ -ErrorAction Ignore | Out-Null

    [System.Net.ServicePointManager]::CertificatePolicy = New-Object TrustAllCertsPolicy
}

[System.Net.ServicePointManager]::CertificatePolicy = New-Object TrustAllCertsPolicy
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Ssl3, [Net.SecurityProtocolType]::Tls, [Net.SecurityProtocolType]::Tls11, [Net.SecurityProtocolType]::Tls12

# Current DateTime for reference to Client State based on Last Seen Time
$currentDateTime = (Get-Date).AddHours(-1) 

# Create function to receive UNIX Time Format and return it for the local timezone

function Convert-UnixTime {
    Param(
        [Parameter(Mandatory = $true)][int32]$unixDate
    )
     
    $orig = (Get-Date -Year 1970 -Month 1 -Day 1 -hour 0 -Minute 0 -Second 0 -Millisecond 0)        
    $timeZone = Get-TimeZone
    $utcTime = $orig.AddSeconds($unixDate)
    $localTime = $utcTime.AddHours($timeZone.BaseUtcOffset.Hours)

    # Return local time
    return $localTime
}
     
$uLogin = Invoke-RestMethod -Method Post -Uri "$($uController)/api/login" -Body ($uAuthBody | convertto-json) -Headers $uHeaders -SessionVariable UBNT

if ($uLogin.meta.rc.Equals("ok")) {
#    Write-Host -ForegroundColor Green "Successfully authenticated to $($uController) as $($uUsername)"

    $uStatusObject = [pscustomobject][ordered]@{ 
        hostname  = $null 
        status    = $null 
        lastHome  = $null  
        homeSince = $null          
        ts = $null
    } 
        
    # Get Registered Clients
    $uClients = Invoke-RestMethod -Method Get -Uri "$($uController)/api/s/$($uSiteID)/stat/alluser" -WebSession $UBNT -Headers $uHeaders
    # Get Active Clients
    $uActiveClients = Invoke-RestMethod -Method Get -Uri "$($uController)/api/s/$($uSiteID)/stat/sta" -WebSession $UBNT -Headers $uHeaders
    # Get devices
    $uDevices = Invoke-RestMethod -Method Get -Uri "$($uController)/api/s/$($uSiteID)/stat/device" -WebSession $UBNT -Headers $uHeaders

    $uClientStatus = @()
    foreach ($uclient in $uInterestingClients) {
        $lastSeen = $null 
        $uAClientState = $null
        $uRClientState = $null 

        $uUser = $uStatusObject.PsObject.Copy()
        $uUser.hostname = $uclient

        # Active Client Data
        $uAClientState = $uActiveClients.data | Select-Object | Where-Object { $_.name -eq $uclient }
        # Registered Client Data
        $uRClientState = $uClients.data | Select-Object | Where-Object { $_.name -eq $uclient }
             
        # if interesting client not found in active clients search all clients
        if ($uAClientState) {        
            $lastSeen = Convert-UnixTime($uAClientState.last_seen)
            if ($lastSeen -ge $currentDateTime.AddMinutes(-5)) {
                $uUser.status = "Home"

                $ap = $uDevices.data | Where-Object {$_.mac -eq $uAClientState.ap_mac}
                $uUser.hostname = "$($uUser.hostname) ($($ap.name))"
            }
            else {
                $uUser.status = "Away"
            }

            $uUser.lastHome = $lastSeen
            $uUser.homeSince = Convert-UnixTime($uAClientState.assoc_time)            
        }
        else {
            $uUser.status = "Away"
            $uUser.lastHome = Convert-UnixTime($uRClientState.last_seen)
        }

        $uClientStatus += $uUser        
    }
#    write-host -ForegroundColor Cyan $uClientStatus
    $uClientStatus | ForEach-Object {
        if ($_.status -eq "Home")
        {
            Write-Host -ForegroundColor DarkGreen $_.hostname -NoNewline
            $ts = (Get-Date).AddHours(-1) - ([datetime]$_.homeSince)
            $tst = "{0:dd}d {0:hh}h {0:mm}m" -f $ts
            if ($ts.Days -eq 0)
            {
                $tst = "{0:hh}h {0:mm}m" -f $ts

                if ($ts.Hours -eq 0)
                {
                    $tst = "{0:mm}m" -f $ts
                }
            }
            Write-Host " - $tst" -ForegroundColor DarkGreen
        }
        else
        {
            Write-Host -ForegroundColor DarkCyan $_.hostname -NoNewline
            $ts = (Get-Date).AddHours(-1) - ([datetime]$_.lastHome)
            $tst = "{0:dd}d {0:hh}h {0:mm}m" -f $ts
            if ($ts.Days -eq 0)
            {
                $tst = "{0:hh}h {0:mm}m" -f $ts

                if ($ts.Hours -eq 0)
                {
                    $tst = "{0:mm}m" -f $ts
                }
            }
            Write-Host " - $tst" -ForegroundColor DarkCyan
        }
    }
}
else {
    Write-Host -ForegroundColor Red "Unsuccessfull in authenticating to $($uController) as $($uUsername)"
}
