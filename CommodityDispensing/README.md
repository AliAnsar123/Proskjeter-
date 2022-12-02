##  Commodity Dispensing
### Group Project in IN5320 - Fall 2022
&nbsp;

# Group Members
## Group 24
* Mohamed Ali Ansar - alians
* Håkon Medhus Fornes - haakonmf
* Oscar Lund Ramstad - oscarlr

# Implemented Functionality
We have implemented the minimum requirements, as well as additional requirement 1 & 3. 

The "surprise" function is Accessibility. The user is able to use keyboard for navigation in the correct order. The only exception to this is in the Dropdown menus (ref. Limitations below).

# Limitations
* Tab indexing doesn't work on Dropdown menus
* There is no validation check on choosing Commodity in Dispensing, so a helpText is used instead.
* API calls on every page instead of just one in App.js

Apart from this, we are not aware of any more limitations.

# Browser Limitation
## Safari
Safari doesn't support a specific regex from Node modules, so preferably use a different browser, such as Edge or Chrome.

# How to run?
1. Clone remote repository to local folder
2. Navigate to folder "CommodityDispensing"
3. Install yarn if it's not already installed by typing `yarn` in the terminal
4. Start the portal by typing `npx dhis-portal --target=https://data.research.dhis2.org/in5320/`. If prompted to install dhis-portal, type ´y´
5. Type `yarn start`

# Available Scripts

In the project directory, you can run:

### `yarn start`

Runs the app in the development mode.<br />
Open [http://localhost:3000](http://localhost:3000) to view it in the browser.

The page will reload if you make edits.<br />
You will also see any lint errors in the console.

### `yarn test`

Launches the test runner and runs all available tests found in `/src`.<br />

See the section about [running tests](https://platform.dhis2.nu/#/scripts/test) for more information.

### `yarn build`

Builds the app for production to the `build` folder.<br />
It correctly bundles React in production mode and optimizes the build for the best performance.

The build is minified and the filenames include the hashes.<br />
A deployable `.zip` file can be found in `build/bundle`!

See the section about [building](https://platform.dhis2.nu/#/scripts/build) for more information.

### `yarn deploy`

Deploys the built app in the `build` folder to a running DHIS2 instance.<br />
This command will prompt you to enter a server URL as well as the username and password of a DHIS2 user with the App Management authority.<br/>
You must run `yarn build` before running `yarn deploy`.<br />

See the section about [deploying](https://platform.dhis2.nu/#/scripts/deploy) for more information.

## Learn More

You can learn more about the platform in the [DHIS2 Application Platform Documentation](https://platform.dhis2.nu/).

You can learn more about the runtime in the [DHIS2 Application Runtime Documentation](https://runtime.dhis2.nu/).

To learn React, check out the [React documentation](https://reactjs.org/).
