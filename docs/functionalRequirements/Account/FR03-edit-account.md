# FR03 - Edit account

## Description
An authenticated user should be able to edit their account information, including username and password. All the fields are optional, only the provided fields will be updated.

## Actors
Authenticated user

## Pre-conditions
- The user is authenticated.

## Main flow
1. User navigates to the account settings page
2. User edits one or more fields.
3. System validates the input data.
4. System updates the account.
6. Visitor is redirected to the account page with a success message.

## Alternatives flows

AF-01 - Username already taken <br>
At step 3, if the provided username is already associated with an existing account, the system display an error message indicating the username is unavailable. The form remains open for correction/submit.

AF-02 - Invalid data <br>
At step 3, if any fields fails validation, the system highlights the offending fields and display descriptive error messages. The account is not created.

AF-03 - Password change requires current password <br>
At step 2, if the user attempts to set a new password, the system requires the current password to be provided as confirmation. If the current password is incorrect, the system displays an error message and the password is not updated.

## Post-conditions
- Account is updated and persisted.
- Visitor is redirected to the account page.

## Requires
- FR02 - Login

## Side-effects

## Priority
Medium

## Status
Draft

## Validations
- Username must be not already registered.
- Password must be at least 4 characters.



