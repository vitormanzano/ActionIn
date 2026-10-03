# FR04 - Delete account

## Description
An authenticated user should be able to delete their account.

## Actors
Authenticated user.

## Pre-conditions
- The user is  authenticated.

## Main flow
1. Visitor navigates to the account settings page.
2. User selects the option to delete their account.
3. System displays a confirmation dialog warning that the action is irreversible.
4. User confirms the deletion by providing the current password.
5. System validates the provided password.
6. System permanently deletes the account.
7. Session is terminated.
8. User is redirected to the sign-up page.

## Alternatives flows

AF-01 - User cancels deletion <br>
At step 3, if the user dismisses the confirmation dialog, the process is aborted and no changes are made.

AF-02 - Incorrect password <br>
At step 5, if the provided password does not match the current account password, the system displays an error message and the account is not deleted.

## Post-conditions
- Account is permanently deleted.
- Session is terminated
- Visitor is redirected to the sign-up page.

## Requires
- FR02 - Login

## Side-effects

## Priority
medium

## Status
Draft

## Validations
- Password must match the current password.


