# FR09 - View Action Details

## Description
An authenticated user should be able to view the details of a specific action

## Actors
Authenticated user

## Pre-conditions
- The user is authenticated.
- The user has performed at least one action
- The user finished the action that they want to see.

## Main flow
1. User navigates to the profile page.
2. The user clicks the "My Actions" button.
3. The system displays the user's actions.
4. The user selects a specific action from the list.
5. The system display the details of the action.

## Alternatives flows

AF01 - Action not found
At step 4, when the user clicks the action from the list, and the system can't get that action, system display an generic alert and return to the actions list.

## Post-conditions

## Requires
- FR05

## Side-effects

## Priority
High

## Status
Draft

## Validations

