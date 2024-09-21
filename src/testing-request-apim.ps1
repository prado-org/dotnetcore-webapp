$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Ocp-Apim-Subscription-Key", "881b3d47a18b4e59887d464995a7f627")

Write-Host "Testing WeatherForecast request to APIM"
1..10 | ForEach-Object {
    echo $_
    $response = Invoke-RestMethod 'https://apim-dotnetproject-dev.azure-api.net/myfirstproject-webapi/api/WeatherForecast' -Method 'GET' -Headers $headers
    #$response | ConvertTo-Json
}

Write-Host "Testing TodoItem request to APIM"
1..10 | ForEach-Object {
    echo $_
    $response = Invoke-RestMethod 'https://apim-dotnetproject-dev.azure-api.net/myfirstproject-webapi/api/TodoItem' -Method 'GET' -Headers $headers
    #$response | ConvertTo-Json
}
