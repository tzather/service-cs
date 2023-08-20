clear

curl -X 'POST' 'http://localhost:5119/Login' \
  --silent \
  --header 'Content-Type: application/json' \
  --data '{
    "userName": "admin@company.com",
    "password": "P@ssw0rd",
    "rememberMe": true,
    "tfa": ""
  }' #| jq .
