git config --global user.name "$GIT_USER_NAME"
git config --global user.email "$GIT_USER_EMAIL"

echo "before"

if [ ! -z "$GIT_REPOSITORY_NAME" ]
then
  echo "GIT_REPOSITORY_NAME = ${GIT_REPOSITORY_NAME}"
  if [ ! -z "$GITHUB_ORGANIZATION" ]
  then
    echo "org flow"
    if [ ! -d "/work/$GITHUB_ORGANIZATION" ]
    then
      echo folder "/work/${GITHUB_ORGANIZATION} doesn't exist... creating"
      mkdir /work/$GITHUB_ORGANIZATION
    fi
    cd /work/$GITHUB_ORGANIZATION
  else
    echo "user flow"
    if [ ! -d "/work/$GITHUB_USER" ]
    then
      echo folder "/work/${GITHUB_USER} doesn't exist... creating"
      mkdir /work/$GITHUB_USER
    fi
    cd /work/$GITHUB_USER
  fi

  echo "completed initial flow"

  if [ ! -d "./$GIT_REPOSITORY_NAME" ]
  then
    echo "repo doesn't exist"
    if [ ! -z "$GITHUB_ORGANIZATION" ]
    then
      echo "org flow 2"
      echo "folder ./${GIT_REPOSITORY_NAME} does not exist cloning"
      REPOSITORY_URL="https://${GITHUB_USER}:${GITHUB_TOKEN}@github.com/${GITHUB_ORGANIZATION}/${GIT_REPOSITORY_NAME}.git"
      git clone ${REPOSITORY_URL}
    else
      echo "user flow 2"
      echo "folder ./${GIT_REPOSITORY_NAME} does not exist cloning"
      REPOSITORY_URL="https://${GITHUB_USER}:${GITHUB_TOKEN}@github.com/${GITHUB_USER}/${GIT_REPOSITORY_NAME}.git"
      git clone ${REPOSITORY_URL}
    fi
  fi

  cd ./$GIT_REPOSITORY_NAME

else
  echo "no repository passed in"
fi

code --no-sandbox --user-data-dir "/vscode-user-data" .
code_pids=$(pidof code)
echo "code_pids = ${code_pids}"
array_of_code_pids=($code_pids)

min=${array_of_code_pids[0]}
max=${array_of_code_pids[0]}

for i in ${array_of_code_pids[*]}; do

  echo "i = {i}"
  # Update max if applicable
  if [[ "$i" -gt "$max" ]]; then
    max="$i"
  fi

  # Update min if applicable
  if [[ "$i" -lt "$min" ]]; then
    min="$i"
  fi
done

 while ps -p $min > /dev/null
 do
    sleep 5
 done
