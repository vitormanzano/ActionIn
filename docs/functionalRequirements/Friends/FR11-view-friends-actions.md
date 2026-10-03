# FR11 - View friends actions

## Description
An authenticated user should be able to view the actions their friends have performed.

## Actors
Authenticated user

## Pre-conditions
- The user is authenticated.
- The user has at least one friend.

## Main flow
1. User navigates to the profile page.
2. The user clicks the "Friends" button.
3. The system display a list of the user's friends.
4. The user selects a specific friend.
5. The system displays the selected friend profile.
6. The user clicks the "My Actions" button.
7. The system displays the actions performed by the selected friend.

## Alternatives flows
AF01 - Friend have no actions performed
At step 3, if the selected friend has not performed any actions, the system will display an alert saying: "No actions were performed"

## Post-conditions

## Requires
- FR10

## Side-effects

## Priority
High

## Status
Draft

## Validations


