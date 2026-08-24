# FR02 - Login

## Description
A user should be able to log in by providing a registered email and the correct password.

## Actors
Visitor (Unauthenticated user)

## Pre-conditions
- The visitor is not authenticated.
- The visitor has a registered account.

## Main flow
1. Visitor navigates to the login page
2. Visitor fills in email and password fields.
3. System validates the input data.
4. System authenticates the user and initiates a session.
6. User is redirected to the main page.

## Alternatives flows

AF-01 - Invalid credentials <br>
At step 3, if the provided email or the password is incorrect, the system should display an ambiguous error message indicating the credentials are incorrect, without specifying which field is wrong. The form remains open for correction.

AF-02 - Blank required fields <br>
At step 3, if any required field is left blank, the system highlights the offending fields and display descriptive error message. Authentication is not performed.

## Post-conditions
- User session is initiated.
- User is redirected to the main page.

## Requires
- FR01 - Create Account

## Side-effects

## Priority
High

## Status
Draft

## Validations
- Email must have be well formatted.
- Password must be at least 4 characters.


