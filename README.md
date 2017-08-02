# GabionPlatform

This is the current series of tools that power GS Lans.  This solution is also slated to help manage devices in my home.  LanPlatform is the centerpiece of the entire operation.  All of the other projects in this solution revolve around LanPlatform in one way or another.

# LanPlatform (aka GabionPlatform)

There are two parts to LanPlatform: the API and the front-end website.  The API is the heart of the entire solution.  Without it, the operation cease to function.  Currently the API only controls local functionality.  In the future it will be expanded to control outside services such as game servers.

The front-end website is the primary user interaction point for the Platform.  Currently under development, it will allow users to navigate all of the features of the Platform.  Admins will also be able to administate the entire platform from the website.  The front-end website is simply a SPA that interfaces with the API.