import React from "react";
import { dataQuery } from "../API/dataQuery";
import { useDataQuery } from "@dhis2/app-runtime";
import { mergeOverviewData } from "../API/mergeData";
import Table from "./Table";
import ExternalOverview from "./ExternalOverview.js";
import Tabs from "./Tabs";
import "../styles.css";
import { CircularLoader } from "@dhis2/ui";



let apiData = [];

export function Overview() {
  const { loading, error, data } = useDataQuery(dataQuery());

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
    apiData = mergeOverviewData(data);
  }
  return (
    <>
      <Tabs>
        <div id="internal" className="internal" label="Internal">
          <div className="container">
            <Table data={apiData} />
          </div>
        </div>

        <div label="External">
          <ExternalOverview />
        </div>
      </Tabs>
    </>
  );
}
