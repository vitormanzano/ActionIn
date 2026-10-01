# How the branch should be created?

## Main branches

main: consider origin/main to be the main branch where the source code of HEAD always reflects a state with the latest delivered development changes for the next release. As a developer, you will be branching and merging from main.

stable: Consider origin/stable to always represent the latest code deployed to production. During day to day development, the stable branch will not be interacted with.

When the source code in the main branch is stable and has been deployed, all of the changes will be merged into stable and tagged with a release number. 

## Supporting Branches
Supporting branches are used to aid parallel development between team members, ease tracking of features, and to assist in quickly fixing live production problems. Unlike the main branches, these branches always have a limited life time, since they will be removed eventually.


### Feature Branches
Feature branches are used when developing a new feature or enhancement which has the potential of a development lifespan longer than a single deployment. When starting development, the deployment in which this feature will be released may not be known. No matter when the feature branch will be finished, it will always be merged back into the main branch.

During the lifespan of the feature development, if there have been commits since the feature was branched. Any and all changes to main should be merged into the feature before merging back to main; this can be done at various times during the project or at the end, but time to handle merge conflicts should be accounted for.

format: <feature/ticketId>

#### How to do it? 
git checkout -b feature/id main                 // creates a local branch for the new feature
git push origin feature/id                        // makes the new feature remotely available

Periodically, changes made to main (if any) should be merged back into your feature branch.

git merge main                                  // merges changes from main into feature branch

When development on the feature is complete, the lead (or engineer in charge) should merge changes into main and then make sure the remote branch is deleted.

git checkout main                               // change to the main branch  
git merge --no-ff feature/id                      // makes sure to create a commit object during merge
git push origin main                            // push merge changes
git push origin :feature/id                      // deletes the remote branch

### Bug Branches
Bug branches differ from feature branches only semantically. Bug branches will be created when there is a bug on the live site that should be fixed and merged into the next deployment. For that reason, a bug branch typically will not last longer than one deployment cycle. Additionally, bug branches are used to explicitly track the difference between bug development and feature development. No matter when the bug branch will be finished, it will always be merged back into main.

if there have been commits since the bug was branched. Any and all changes to main should be merged into the bug before merging back to main; this can be done at various times during the project or at the end, but time to handle merge conflicts should be accounted for.

format: <bugfix/ticketId>

#### How to do it?
git checkout -b bugfix/id main                     // creates a local branch for the new bug
git push origin bugfix/id                            // makes the new bug remotely available

Periodically, changes made to main (if any) should be merged back into your bug branch.

git merge main                                  // merges changes from main into bug branch

When development on the bug is complete, [the Lead] should merge changes into main and then make sure the remote branch is deleted.

git checkout main                               // change to the main branch  
git merge --no-ff bugfix/id                          // makes sure to create a commit object during merge
git push origin main                            // push merge changes
git push origin :bugfix/id                           // deletes the remote branch




