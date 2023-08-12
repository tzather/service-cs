clear

curl -X 'POST' 'http://localhost:5119/Login' \
  --silent \
  --header 'Content-Type: application/json' \
  --data '{
    "userName": "admin@company.com",
    "password": "P@ssw0rds",
    "rememberMe": true,
    "tfa": ""
  }' #| jq .
