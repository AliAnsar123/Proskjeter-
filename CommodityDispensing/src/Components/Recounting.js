import React, { useState } from "react";
import { recountQuery } from "../API/dataQuery";
import { mergeRecountingData } from "../API/mergeData";
import { sendRecount, sendRecountLog } from "../API/mutationQuery";
import Tabs from "./Tabs";
import "../styles.css";
import { useDataQuery, useDataMutation } from "@dhis2/app-runtime";
import { RecountingLog } from "./RecountingLog";
import {
  DataTable,
  Button,
  TableBody,
  DataTableCell,
  DataTableColumnHeader,
  TableHead,
  DataTableRow,
  IconCheckmarkCircle24,
  AlertBar,
} from "@dhis2/ui";

let apiData = [];

export function Recounting() {
  const [mutate] = useDataMutation(sendRecount());
  const [mutateDataStore] = useDataMutation(sendRecountLog());

  let [alertBar, setAlertBar] = useState("");

  function updateAll() {
    // Making a list from the old data on the dataStore
    let logList = data.dataStore;
    let loopTime = 0;
    // going through the apidata
    for (let i = 0; i < apiData.length; i++) {
      const balanceInput = document.getElementById(apiData[i].id).value;
      // If the input box has any number, then the data is sent to the api
      if (balanceInput.length !== 0) {
        loopTime++;
        //If negative value, then give alert
        if (balanceInput < 0) {
          alert("only positive values");
          return;
        }

        mutate({
          categoryOptionCombo: "rQLFnNXXIL0",
          dataElement: apiData[i].id,
          period: "202110",
          orgUnit: "uYG1rUdsJJi",
          value: balanceInput,
        });

        // Creating a log for the DataStore
        const commodityName = apiData[i].name.substring(
          14,
          apiData[i].name.length
        );
        const commoditycategory = apiData[i].category.substring(
          12,
          apiData[i].category.length
        );
        const today = new Date();
        const date = today.toLocaleDateString("de-DE");
        const time =
          today.getHours() +
          ":" +
          (today.getMinutes() < 10 ? "0" : "") +
          today.getMinutes();
        const id = logList.length + 1;

        logList.push({
          id: id,
          date: date,
          time: time,
          name: commodityName,
          category: commoditycategory,
          oldValue: apiData[i].endBalance,
          newValue: balanceInput,
        });
      }
    }
    if (loopTime == 0) {
      alert("No balance in the input boxes");
      return;
    } else {
      mutateDataStore({ logList });

      setAlertBar(
        <AlertBar className="alertbar" permanent success>
          Success
        </AlertBar>
      );

      //Waiting 2 secounds and then reloading the screen so that the user can se the confirmation alert
      function delay(time) {
        return new Promise((resolve) => setTimeout(resolve, time));
      }
      delay(2000).then(() => window.location.reload());
    }
  }

  const { loading, error, data } = useDataQuery(recountQuery());
  if (error) {
    return <span>ERROR: {error.message}</span>;
  }

  if (loading) {
    return <span>Loading...</span>;
  }

  if (data) {
    const mergedData = mergeRecountingData(data);
    mergedData.sort((a, b) => (a.name > b.name ? 1 : -1));
    apiData = mergedData;

    return (
      <Tabs>
        <div className="restock-tab" label="Recounting">
          {alertBar}
          <div className="restocking">
            <DataTable className="restocking-table">
              <TableHead>
                <DataTableRow>
                  <DataTableColumnHeader>Name</DataTableColumnHeader>
                  <DataTableColumnHeader>End balance</DataTableColumnHeader>
                  <DataTableColumnHeader>
                    New Balance
                    <Button
                      primary
                      className="update-button"
                      icon={<IconCheckmarkCircle24 />}
                      onClick={() => updateAll()}
                    >
                      Send
                    </Button>
                  </DataTableColumnHeader>
                </DataTableRow>
              </TableHead>
              <TableBody>
                {mergedData.map(({ id, name, endBalance }) => (
                  <DataTableRow key={name}>
                    <DataTableCell>
                      {name.substring(14, name.length)}
                    </DataTableCell>
                    <DataTableCell>{endBalance}</DataTableCell>
                    <DataTableCell>
                      <input id={id} name="number" type="number" />
                    </DataTableCell>
                  </DataTableRow>
                ))}
              </TableBody>
            </DataTable>
          </div>
        </div>

        <div className="restock-log" label="Log">
          <RecountingLog />
        </div>
      </Tabs>
    );
  }
}
