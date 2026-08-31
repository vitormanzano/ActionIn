# FR06 - Finish action

## Description
An authenticated user should be able to stop a action.

## Actors
Authenticated user

## Pre-conditions
- The user is authenticated.
- The user is current doing some action.

## Main flow
1. User navigates to the main page
2. User clicks done button.
4. User clicks the button confirm.
5. System validates the data.
6. System creates the action.
7. User is redirected to the main page.

## Alternatives flows

AF-01 - Blank required fields <br>
At step 3, if any required field is left blank, the system highlights the offending fields and display an descriptive error message. 

AF-02 - User cancel the create <br>
At step 4, if the user clicks to cancel the create action, the process is aborted and the action is not created.

## Post-conditions
- Action is finished.
- Visitor is redirected to the main page.

## Requires
- FR02

## Side-effects

## Priority
High

## Status
Draft

## Validations
- Title must have at least 4 characters.


