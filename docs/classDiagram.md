# Class diagram

## Account
### Fields
- Id
- Email
- Username
- Password
### Methods
- Register
- Login
- ValidateUsername
- ValidateEmail
- ValidatePassword
- EditUsername
- EditPassword

## Profile
### Fields
- UserId
- Actions
### Methods
- GetAllActions


## Relations
### Fields
- Id
- UserA
- UserB
- Status
### Methods
- SendFriendRequest
- AcceptFriendRequest
- RejectFriendRequest
- EndFriendship

## Action
### Fields
- Id
- UserId
- Title
- Description
- StartedAt
- FinishedAt
### Methods
- StartAction
- FinishAction
- GetActionTime



