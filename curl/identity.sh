clear

# curl -X 'POST' 'http://localhost:5119/Login' \
#   --silent \
#   --header 'Content-Type: application/json' \
#   --data '{
#     "userName": "admin@company.com",
#     "password": "P@ssw0rda",
#     "rememberMe": true,
#     "tfa": ""
#   }' #| jq .

curl -X 'GET' 'http://localhost:5119/Login' \
  --silent \
  --header 'Content-Type: application/json'
echo ''

curl -X 'GET' 'http://localhost:5119/Login' \
  --silent \
  --header 'accept: text/csv'
echo ''
