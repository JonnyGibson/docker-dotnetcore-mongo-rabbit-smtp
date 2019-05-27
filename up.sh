
case "$OSTYPE" in
  solaris*) echo "SOLARIS" ;;
  darwin*)  
  echo "OSX"
    open "http://localhost:8888/swagger"
    open "http://localhost:8081"
    pen "http://localhost:8025"
    open "http://localhost:15672" ;; 
  linux*)   echo "LINUX" ;;
  bsd*)     echo "BSD" ;;
  msys*)    
  echo "WINDOWS"
  start "http://localhost:8888/swagger"
    start "http://localhost:8081"
    start "http://localhost:8025"
    start "http://localhost:15672" ;;
  *)        echo "unknown: $OSTYPE" ;;
esac
docker-compose up