# Introduction 
This is a stater-kit to begin work with Azure Auth functionality in Xamarin Apps. Azure AD is an indentity provider which allows signing-in with your organisation credentials (single-tenant) or multiple other credentials (multi-tenant). All things related to authenticating users will be handled by Azure AD. As well, it's got SSO (Single-Sing-On) built-in. 

# Prerequisites 

Register an app in Azure AD for mobile clients. 
- Log in to portal.azure.com 
- Select Azure Active Directory.
- Select App Registrations.
- Click on New Registration and proceed as Public native/client app. 
- Add platform specific redirect urls. [https://docs.microsoft.com/en-us/azure/active-directory/develop/tutorial-v2-android] 
- Grant Admin consent.
- **Enter the correct Azure AD params in the project.**

# Getting Started
Run the Android project in solution.
