# FR01 - Create account

## Description
An anonymous user should be able to create an account by providing a username, email and password.

## Actors
Visitor (Unauthenticated user)

## Pre-conditions
- The visitor is not authenticated.
- The provided email is not already registered.
- The provided username is not already taken.

## Main flow
1. Visitor navigates to the sign-up page
2. Visitor fills in username, email and password fields.
3. System validates the input data.
4. System creates the account.
5. System sends a welcome email to the provided email.
6. Visitor is redirected to the login page.

## Alternatives flows

AF-01 - Email already registered <br>
At step 3, if the provided email is already associated with an existing account, the system display an error message indicating the email is unavailable. The form remains open for correction/submit.

AF-02 - Username already taken <br>
At step 3, if the provided username is already associated with an existing account, the system display an error message indicating the username is unavailable. The form remains open for correction/submit.

AF-03 - Invalid data <br>
At step 3, if any fields fails validation, the system highlights the offending fields and display descriptive error messages. The account is not created.

## Post-conditions
- Account is created and persisted.
- Visitor is redirected to the login page.

## Requires
-

## Side-effects
- A welcome email is sent to the registered email.

## Priority
High

## Status
Draft

## Validations
- Username must be not already registered.
- Email must have be well formatted.
- Email must be not already registered.
- Password must be at least 4 characters.

