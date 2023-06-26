#! /bin/bash

# set path to current folder
cd "$(dirname "$0")"

# run dockeragent locally
../../devops-azure/dockeragent/create-local.sh
