import React from "react";
import { useDataQuery } from "@dhis2/app-runtime";
import { mergeExternalData } from "../API/mergeData";
import Table from "./Table";
import "../styles.css";
import { orgQuery } from "../API/dataQuery";
import {
  Menu,
  CircularLoader,
  MenuItem,
  DropdownButton,
  FlyoutMenu,
} from "@dhis2/ui";

let currentOrg = "";
let orgData = "";

export default function Request() {
  const { loading, error, data, refetch } = useDataQuery(orgQuery(), {
    variables: {
      orgUnit: "uYG1rUdsJJi",
    },
  });

  function viewTable(id, name) {
    refetch({ orgUnit: id });
    if (error) return <span>ERROR: {error.message}</span>;

    if (loading) return <CircularLoader large />;

    if (data) {
      // if the orgunit has no data
      if (data.dataValueSets.dataValues.length < 1) {
        return <span>No data for this organization</span>;
      }
      orgData = mergeExternalData(data);
      currentOrg = name;
    }
  }
  if (error) {
    return <span>ERROR: {error.message}</span>;
  }

  if (loading) {
    return (
      <span>
        <CircularLoader large />
      </span>
    );
  }

  if (data) {
    return (
      <div>
        <div class="external">
          <DropdownButton
            component={
              <FlyoutMenu maxHeight="300px">
                <Menu>
                  {data.orgSet.organisationUnits.map((c) => (
                    <MenuItem dense
                      key={c.id}
                      label={c.displayName}
                      onClick={() => viewTable(c.id, c.displayName)}
                    />
                  ))}
                </Menu>
              </FlyoutMenu>
            }
          >
            {currentOrg ? currentOrg : "Select an organization"}
          </DropdownButton>
        </div>
        <Table data={orgData} />
      </div>
    );
  } else {
    return <p>No data</p>;
  }
}
