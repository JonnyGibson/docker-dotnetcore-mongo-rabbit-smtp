case "$OSTYPE" in
solaris*) echo "SOLARIS" ;;
darwin*)
  echo "OSX"
  open "http://localhost:5000"
  sudo dotnet run -p frontend
  ;;
linux*) echo "LINUX" ;;
bsd*) echo "BSD" ;;
msys*)
  echo "WINDOWS"
  start "http://localhost:5000"
  dotnet run -p frontend
  ;;
*) echo "unknown: $OSTYPE" ;;
esac