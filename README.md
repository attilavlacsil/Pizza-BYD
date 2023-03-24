# Pizza-BYD

## Workflows

1. CI: On push on develop branch: Build and test the solution
1. Prerelease: On prerelease: Build and push a new image with **rc** and **vX.Y.Z-rc** tag
1. End-to-end test: After a prerelease or manual start: Start the latest rc release and execute end-to-end tests
