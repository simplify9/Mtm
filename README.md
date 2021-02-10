| **Package**       | **Version** |
| :----------------:|:----------------------:|
| [`SimplyWorks.MTM`](https://www.nuget.org/packages/SimplyWorks.Mtm.Sdk/)|![Nuget](https://img.shields.io/nuget/v/SimplyWorks.Mtm.Sdk?style=for-the-badge)|

![License](https://img.shields.io/badge/license-MIT-blue.svg)

## Introduction 
_MTM_ is a multi-tenant management system that handles setting up users, authenticating them, and authorizing them for program access. 

As the owner of a tenant, _MTM_ allows for you to invite an employee, making them users under your tenancy. 
To authenticate a user, _MTM_ returns a [JSON Web Token (JWT)](https://jwt.io), identifying the user's permissions. 

## MTM SDK
The [MTM Client](https://github.com/simplify9/Mtm/blob/master/SW.Mtm.Sdk/MtmClient.cs) outlines the steps the user takes to enable _MTM_ onto the program and interact with its many properties. 
To complete a cycle on _MTM_, call the following functions:
 
#### 1- Creating the tenant 
The first step is to create a tenant by calling the `CreateTenant` method provided by the SDK, which takes in details in the model of `TenantCreate` and includes the tenant's name for example.

#### 2- Accepting the invitation 
The second step is to accept the invitation by calling `AcceptInvitation` method provided by the SDK, which takes in the details by mode of `InvitationAccept`. 

#### 3- Inviting user to tenancy 
Then invite the user to reside under the tenancy, calling `Invite`. Given the user accepts the invitation, we can move on to the following step in the cycle.

#### 4- LOGIN
The user logs in, prompting `Login` (which takes in details in the `AccountLogin` model) using their outlined credentials, previously returned by the _MTM_-generated token. 

#### 5- Account Creation 
This is when _MTM_ completes the cycle, creating for the logged-in user an account, indentifying their role, and enabling access onto the program under the tenancy. 
Prompt `CreateAccount` and your user is ready. 

## Additional Properties
Like with most user-based programs, _MTM_ allows users a variety of properties to modify the program's use.
(*Note: the properties marked with an asterisk are limited to the Administrator and Moderators' use*.)

#### - Changing password on user's end

#### - Resetting the password

#### - Seeking sent invitation to user*

#### - Retrieving invitation*

#### - Canceling invite*

#### - Switching Tenants*

#### - Removing account*

#### - Adding an account*

#### - Setting up profile data

#### - Searching Accounts 

## Getting support 👷
If you encounter any bugs, don't hesitate to submit an [issue](https://github.com/simplify9/Mtm/issues). We'll get back to you promptly! 
