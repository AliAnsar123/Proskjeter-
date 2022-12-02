import React from "react";
import classes from "./App.module.css";
import { useState } from "react";

import { Dispensing } from "./Components/Dispensing";
import { Recounting } from "./Components/Recounting";
import { Overview } from "./Components/Overview";
import { Menu, MenuItem } from "@dhis2/ui";
import { IconExportItems24, IconImportItems24, IconHome24 } from "@dhis2/ui";

function MyApp() {
  const [activePage, setActivePage] = useState("Overview");

  function activePageHandler(page) {
    setActivePage(page);
  }

  return (
    <div className={classes.container}>
      <div className={classes.left}>
        <Menu>
          <div
            onKeyDown={(e) => {
              if (e.key === "Enter") {
                activePageHandler("Overview");
              }
            }}
            tabIndex={0}
          >
            <MenuItem
              label="Overview"
              icon={<IconHome24 />}
              active={activePage == "Overview"}
              onClick={() => activePageHandler("Overview")}
            />
          </div>

          <div
            onKeyDown={(e) => {
              if (e.key === "Enter") {
                activePageHandler("Recounting");
              }
            }}
            tabIndex={0}
          >
            <MenuItem
              label="Recounting"
              icon={<IconImportItems24 />}
              active={activePage == "Recounting"}
              onClick={() => activePageHandler("Recounting")}
            />
          </div>

          <div
            onKeyDown={(e) => {
              if (e.key === "Enter") {
                activePageHandler("Dispensing");
              }
            }}
            tabIndex={0}
          >
            <MenuItem
              label="Dispensing"
              icon={<IconExportItems24 />}
              active={activePage == "Dispensing"}
              onClick={() => activePageHandler("Dispensing")}
            />
          </div>
        </Menu>
      </div>
      <div className={classes.right}>
        {activePage === "Overview" && <Overview />}
        {activePage === "Dispensing" && <Dispensing />}
        {activePage === "Recounting" && <Recounting />}
      </div>
    </div>
  );
}
export default MyApp;
