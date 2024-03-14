# Git

## Windows

Download Git from https://git-scm.com/download/win and install. 

Once installed changed your email and name:

```shell
git config --list
git config user.name "Your Name"
git config user.email "your_email@example.com"
```

If you want to set it globally, pass in the -

## Mac

Setup by default :)

## Linux

You already know :)

## Create a new Repo on GitHub

1. Login to GitHub
2. Click on the Repositories Tab
3. Click the New Repository Button
4. Give it a Name
5. Add a README file by clicking the checkbox
6. Click Create

## Create an SSH Key Locally
https://docs.github.com/en/authentication/connecting-to-github-with-ssh/generating-a-new-ssh-key-and-adding-it-to-the-ssh-agent


## Create a new git repo locally

1. First we need to step into the root of the project
2. Once inside we need to do two things, first is to generate a gitignore file and the next is the initialize the folder as a git project:
```shell
dotnet new gitignore
git init
```
3. Next, we need to point the remote origin to the Github URL
4. Go back to Github and click into the respository
5. Click on the Code button (green in colour), choose SSH and copy the path: **git@github.com:<YOUR NAME>/ECommerceApp.git**
6. Next, we need to add the URL to our origin in Git

```shell
git remote add origin <remote_repository_URL>
```
