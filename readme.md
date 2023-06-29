[how-to-scan-nuget-packages-for-security-vulnerabilities](https://devblogs.microsoft.com/nuget/how-to-scan-nuget-packages-for-security-vulnerabilities)

- dotnet list package --vulnerable
- dotnet list package --vulnerable --include-transitive
- dotnet list package --include-transitive

### Axure Pipeline vulnerability

- https://www.mytechramblings.com/posts/check-if-your-dotnet-app-dependencies-has-a-security-vulnerability-on-you-cicd-pipelines/
- https://dejanstojanovic.net/aspnet/2022/june/using-dotnet-nuget-package-vulnerability-scan-in-azure-devops-build/

```
dotnet list package --vulnerable --include-transitive >build.log
dotnet list package --vulnerable --include-transitive 2>&1 | tee build.log

echo "Analyze dotnet list package command log output..."
grep -q -i "critical\|high\|moderate\|low" build.log; [ $? -eq 0 ] && echo "Security Vulnerabilities found on the log output" && exit 1

```
