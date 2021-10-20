# from https://blog.darrenjrobinson.com/accessing-your-ubiquiti-unifi-network-configuration-with-powershell/

# Unifi Controller Login Base URI
$uController = 'https://192.168.4.2:8443' # e.g 'https://192.168.1.2:8443'
# Identifier of the site in UniFi. Set to default for the default site
$uSiteID = "default"

$uUsername = 'ms2n' # yourAdmin UserID
$uPassword = '2008InspectionReport!' # yourAdmin User Password
$uAuthBody = @{"username" = $uUsername; "password" = $uPassword }
$uHeaders = @{"Content-Type" = "application/json" }

# Allow connection with the Unifi Self Signed Cert
add-type @"
using System.Net;
using System.Security.Cryptography.X509Certificates;
public class TrustAllCertsPolicy : System.Net.ICertificatePolicy {
    public bool CheckValidationResult(
        ServicePoint srvPoint, X509Certificate certificate,
        WebRequest request, int certificateProblem) {
        return true;
    }
}
"@
[System.Net.ServicePointManager]::CertificatePolicy = New-Object TrustAllCertsPolicy
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Ssl3, [Net.SecurityProtocolType]::Tls, [Net.SecurityProtocolType]::Tls11, [Net.SecurityProtocolType]::Tls12

$uLogin = Invoke-RestMethod -Method Post -Uri "$($uController)/api/login" -Body ($uAuthBody | convertto-json) -Headers $uHeaders -SessionVariable UBNT

if ($uLogin.meta.rc.Equals("ok")) {
    Write-Host -ForegroundColor Green "Successfully authenticated to $($uController) as $($uUsername)"
    
    # Get Devices
    $uDevices = Invoke-RestMethod -Method Get -Uri "$($uController)/api/s/$($uSiteID)/stat/device" -WebSession $UBNT -Headers $uHeaders
    write-host -ForegroundColor cyan "Devices"
    $uDevices.data

}
else {
    Write-Host -ForegroundColor Red "Unsuccessfull in authenticating to $($uController) as $($uUsername)"
}