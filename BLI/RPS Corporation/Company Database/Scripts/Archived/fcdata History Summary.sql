select si.ItemText, Count(HistoryID) AS HistCount
from tblMenuHistory mh, [Switchboard Items] si
where si.SwitchboardID = mh.HistorySwitchID
	and si.ItemNumber = mh.HistorySwitchItem
group by si.ItemText
order by HistCount DESC