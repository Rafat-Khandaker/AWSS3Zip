SELECT * FROM IISLogEvents 

SELECT DateTime, 
substr(RequestMessage, instr(RequestMessage, 'Key='), 16) AS UserKey,
substr(RequestMessage, instr(RequestMessage, 'GET '), length(substr(RequestMessage, 1, instr(RequestMessage, ' 80') -1))) AS HttpRequest 
FROM IISLogEvents 
WHERE RequestMessage like '%Geoservice/Geoservice.svc%' 
AND UserKey like '%Key=%'

SELECT 
substr(RequestMessage, instr(RequestMessage, 'Key='), 16) AS UserKey, COUNT(*) AS Call_Count 
FROM IISLogEvents 
WHERE RequestMessage like '%Geoservice/Geoservice.svc%' 
AND UserKey like '%Key=%'
GROUP BY UserKey
ORDER BY Call_Count DESC;