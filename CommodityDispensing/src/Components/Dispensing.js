import React, { useState } from "react";
import { useDataQuery, useDataMutation } from "@dhis2/app-runtime";
import { dispensingDataQuery } from "../API/dataQuery";
import { sendDispens, sendDispensLog  } from "../API/mutationQuery";
import { mergeDispensData } from "../API/mergeData";
import "../styles.css";
import { v4 as UUID } from "uuid";
import { DispensingLog } from "./DispensingLog.js"
import Tabs from "./Tabs.js"
import {
  ReactFinalForm,
  SingleSelectOption,
  SingleSelectField,
  CircularLoader,
  Button,
  InputFieldFF,
  composeValidators,
  hasValue,
  number,
  createMinNumber,
  IconAdd24,
  IconDelete24,
  IconExportItems24,
  colors,
  Table,
  TableBody,
  TableCell,
  TableCellHead,
  TableHead,
  TableRow,
  TableRowHead,
  AlertBar,
} from "@dhis2/ui";

let pendingLog = []; 

export function Dispensing() {
  const { loading, error, data, refetch } = useDataQuery(dispensingDataQuery());
  const [mutate] = useDataMutation(sendDispens());
  const [mutateDataStore] = useDataMutation(sendDispensLog());

  let [tableData, setTableData] = useState([])
  let [table, setTable] = useState([])
  let [alertBar, setAlertBar] = useState("")


  const sendCommodities = async (list, log) => {

    if (list.length == 0) {
      alert("You must add commodities to send.");
      return;
    }

    let mutation = []

    let refetched
    try {
      refetched = await refetch();
    } catch {
      return new Promise(function (reject) {
        reject("Could not get the data again!");
      });
    }

    const refetchedDataValues = refetched.dataValueSets.dataValues;


    let dupCommodities = []
    for (let i = 0; i<list.length;i++ ){

      // If there are duplicate commodities, then making a sum of them
      if (!dupCommodities.includes(list[i].name)){

      let sameList = list.filter(a => a.name == list[i].name);
      dupCommodities.push(list[i].name)
      
      // Finding the sum of amount that is going to be sent
      let sumSend = 0;
      sameList.forEach(element => {
        sumSend += parseInt(element.sendAmount);
      });
      console.log(sumSend)
      const balanceValue = refetchedDataValues.find((obj) => {
        return (
          obj.dataElement === list[i].id &&
          obj.categoryOptionCombo == "rQLFnNXXIL0"
        );
      }).value;

      const consumptionValue = refetchedDataValues.find((obj) => {
        return (
          obj.dataElement === list[i].id &&
          obj.categoryOptionCombo == "J2Qf1jtZuj8"
        );
      }).value;
      console.log(balanceValue)
      console.log(consumptionValue)
      const balance = parseInt(balanceValue) - sumSend;
      const consumption = parseInt(consumptionValue) + sumSend;
      console.log(balance)
      console.log(consumption)
      mutation.push({
        categoryOptionCombo: "rQLFnNXXIL0",
        dataElement: list[i].id,
        period: "202110",
        orgUnit: "uYG1rUdsJJi",
        value: balance,
      });

      mutation.push({
        categoryOptionCombo: "J2Qf1jtZuj8",
        dataElement: list[i].id,
        period: "202110",
        orgUnit: "uYG1rUdsJJi",
        value: consumption,
      });
    };

    mutate({ commoditiesMutation: mutation })
      .then(function (response) {
        if (response.status !== "SUCCESS") console.log(response);
      })
      .catch(function (response) {
        console.log(response);
      });
    }
      const logList = data.dataStore

      const id = data.dataStore.length+1;
      // Adding a id to the Log List
      const logWithId = log.map( (data, index) => ({...data, id:index+id}) );

      // Going through the log list, and combing it with the existing data on the datastore
      for (let i = 0; i< logWithId.length; i++){
        logList.push(logWithId[i])
      }
      mutateDataStore({logList})
      setAlertBar((<AlertBar permanent success>Success</AlertBar>));

      //Waiting 2 secounds and then reloading the screen so that the user can se the confirmation alert
      function delay(time) {
        return new Promise(resolve => setTimeout(resolve, time));
      }
      delay(2000).then(() =>     window.location.reload());
  };



  if (error) return <span>ERROR: {error.message}</span>;

  if (loading) return <CircularLoader large />;

  if (data) {
    let res = mergeDispensData(data);

    let commodities = [];
    res.map((r) =>
      commodities.push({
        key: r.id,
        label: r.name,
        value: r.value,
        orgUnit: r.orgUnit,
      })
    );
    commodities.sort((a, b) => (a.label > b.label ? 1 : -1))

    // Called when a new commodity is added
    const onSubmit = (commodity) => {

      let pendingCommodities = tableData;

      commodities.find((c) => {
        if (c.key === commodity.id) {
          pendingCommodities.push({
            id: c.key,
            name: c.label,
            orgUnit: c.orgUnit,
            value: c.value,
            hospital: commodity.hospital,
            recipient: commodity.recipient,
            sendAmount: commodity.sendAmount,
          });
          setTableData(pendingCommodities)

          const commodityName =  c.label.substring(
            14,
            c.label.length
          );
          const today = new Date();
          const date = today.toLocaleDateString("de-DE");
          const time = today.getHours() + ":" + (today.getMinutes()<10?'0':'') + today.getMinutes();

          pendingLog.push({
            commodity: commodityName,
            date: date,
            time: time,
            hospital: commodity.hospital,
            recipient: commodity.recipient,
            sendAmount: commodity.sendAmount,
          });
        }
      });
      updateTable();
    };

    function updateTable(){
      setTable(
        <TableBody>
            {tableData.map((pendingCommodity, index) => (
              <TableRow key={pendingCommodity.id + UUID()}>
                <TableCell>{pendingCommodity.recipient}</TableCell>
                <TableCell>{pendingCommodity.hospital}</TableCell>
                <TableCell>
                  {pendingCommodity.name.substring(
                    14,
                    pendingCommodity.name.length
                  )}
                </TableCell>
                <TableCell>{pendingCommodity.sendAmount}</TableCell>
                <TableCell>
                  <Button
                    small
                    icon={<IconDelete24 color={colors.red500} />}
                    value="Delete"
                    onClick={() => {
                      tableData.splice(index, 1);
                      pendingLog.splice(index, 1);
                      updateTable();
                    }}
                  />
                </TableCell>
              </TableRow>
            ))}
          </TableBody>)
    }


    return (
      <Tabs>
        <div label="Dispensing">
          {alertBar}
            <section className="dispensing">
              <ReactFinalForm.Form onSubmit={onSubmit}>
                {({ handleSubmit }) => (
                  <form onSubmit={handleSubmit} autoComplete="off">
                    <ReactFinalForm.Field
                      required
                      component={InputFieldFF}
                      validate={composeValidators(hasValue)}
                      name="recipient"
                      label="Send to person:"
                    ></ReactFinalForm.Field>
                    <ReactFinalForm.Field
                      required
                      component={InputFieldFF}
                      validate={composeValidators(hasValue)}
                      name="hospital"
                      label="At Hospital:"
                    ></ReactFinalForm.Field>
                    <ReactFinalForm.Field
                      required
                      name="id"
                      validate={composeValidators(hasValue)}>
                      {(item) => (
                        <SingleSelectField
                          required
                          dense
                          label="Commodity:"
                          helpText="Remember to choose commodity!"
                          name={item.input.name}
                          selected={item.input.value}
                          onChange={(c) => item.input.onChange(c.selected)}>
                          {commodities.map((c) => (
                            <SingleSelectOption
                              required
                              key={c.key}
                              label={c.label.substring(14, c.label.length)}
                              value={c.key}>
                            </SingleSelectOption>
                          ))}
                        </SingleSelectField>
                      )}
                    </ReactFinalForm.Field>
                    <ReactFinalForm.Field
                      required
                      component={InputFieldFF}
                      validate={composeValidators(
                        hasValue,
                        number,
                        createMinNumber(1)
                      )}
                      name="sendAmount"
                      label="Amount to Send:">
                    </ReactFinalForm.Field>
                    {tableData.length > 0 ? (
                      <section>
                        <Button
                          className="commodityFormButtons"
                          icon={<IconAdd24 />}
                          type="submit">
                          Add
                        </Button>
                        <Button
                          primary
                          icon={<IconExportItems24 />}
                          onClick={() => sendCommodities(tableData,pendingLog)}>
                          Send
                        </Button>
                        <Table>
                          <TableHead>
                            <TableRowHead>
                              <TableCellHead colSpan="4">
                                Commodities to be Sent
                              </TableCellHead>
                            </TableRowHead>
                            <TableRowHead>
                              <TableCellHead>Recipient</TableCellHead>
                              <TableCellHead>Hospital</TableCellHead>
                              <TableCellHead>Commodity</TableCellHead>
                              <TableCellHead>Amount</TableCellHead>
                            </TableRowHead>
                          </TableHead>
                          {table}
                        </Table>
                        <Button
                          className="deleteAllButton"
                          small
                          icon={<IconDelete24 color={colors.red800} />}
                          onClick={() => {
                            setTableData([])
                            pendingLog = []
                          }}>
                          Delete All
                        </Button>
                      </section>
                      ) : (
                      <section>
                        <Button
                          className="commodityFormButtons"
                          icon={<IconAdd24 />}
                          type="submit">
                          Add
                        </Button>
                      </section>
                      )
                    }
                  </form>
                )}
              </ReactFinalForm.Form>
            </section>
          </div>
          <div label="Log">
            <DispensingLog />
          </div>
        </Tabs>
      );
    }
}
