| **Package**       | **Version** |
| :----------------:|:----------------------:|
| [`SimplyWorks.MTM`](https://www.nuget.org/packages/SimplyWorks.Mtm.Sdk/)|![Nuget](https://img.shields.io/nuget/v/SimplyWorks.Mtm.Sdk?style=for-the-badge)|

![License](https://img.shields.io/badge/license-MIT-blue.svg)

## Introduction 
_MTM_ is a multi-tenant management system that handles setting up users, authenticating them, and authorizing them for program access. 

As the owner of a tenant, _MTM_ allows for you to invite an employee, making them users under your tenancy. 
To authenticate a user, _MTM_ returns a [JSON Web Token (JWT)](https://jwt.io), identifying the user's permissions. 

## MTM SDK
The [MTM Client](https://github.com/simplify9/Mtm/blob/master/SW.Mtm.Sdk/MtmClient.cs) outlines the steps the user takes to enable _MTM_ onto the program and interact with its many properties:
 
#### Creating the tenant 
The first step is to create a tenant by calling the `CreateTenant` method provided by the SDK, which takes in details in the model of `TenantCreate` and includes the tenant's name for example.

#### Accepting the invitation 

#### Inviting user to tenancy 

#### LOGIN prompted

#### LOGIN inputted on user's end

#### MTM Then creates an account for the user

#### Changing password on user's end:

#### Resetting the password:

#### MTM Initiating Password Reset 

#### Seeking sent invitation to user

#### Retrieving invitation

#### Canceling invite 

#### Switching Tenants 

#### Removing account 

#### Adding an account 

#### Setting up profile data

#### Searching Accounts 

## Getting support 👷
If you encounter any bugs, don't hesitate to submit an [issue](https://github.com/simplify9/Mtm/issues). We'll get back to you promptly! 
