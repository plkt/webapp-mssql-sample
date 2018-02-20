CREATE TABLE accessLogs
(
   PageName  nvarchar(128)     not null, 
    [AccessDate]  DATETIME NOT NULL DEFAULT GETUTCDATE() 
);
