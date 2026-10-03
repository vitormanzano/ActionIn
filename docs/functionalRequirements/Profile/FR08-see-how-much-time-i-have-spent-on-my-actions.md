# FR08 - See how much time i've spent on my actions

## Description
An authenticated user should be able to see the total time they have spent performing all actions.

## Actors
Authenticated user

## Pre-conditions
- The user is authenticated.

## Main flow
1. The user navigates to the profile page.
2. The user clicks the "My Actions" button.
3. The system displays the user's actions.
4. The system displays at the top of the actions list, the total time spent performing them, formatted in hours and minutes.

## Alternatives flows

AF-01 - No actions performed 
At step 3, if the user has not performed any actions, the system will display an alert saying: "No actions were performed". 

## Post-conditions

## Requires
- FR02

## Side-effects

## Priority
High

## Status
Draft

## Validations




