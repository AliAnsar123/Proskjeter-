import React from "react";
import { useDataQuery } from "@dhis2/app-runtime";
import { dispensDataStoreQuery } from "../API/dataQuery";
import "../styles.css";
import {
  DataTable,
  DataTableCell,
  DataTableColumnHeader,
  DataTableRow,
  TableHead,
  TableBody,
  CircularLoader,
} from "@dhis2/ui";

export function DispensingLog() {
  const { loading, error, data } = useDataQuery(dispensDataStoreQuery());

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
      <>
        <div>
          <DataTable className="restocking">
            <TableHead>
              <DataTableRow>
                <DataTableColumnHeader name="Commodity">Commodity</DataTableColumnHeader>
                <DataTableColumnHeader name="Date"> Date</DataTableColumnHeader>
                <DataTableColumnHeader name="Time">Time</DataTableColumnHeader>
                <DataTableColumnHeader name="Hospital">Hospital</DataTableColumnHeader>
                <DataTableColumnHeader name="Recipient">Recipient</DataTableColumnHeader>
                <DataTableColumnHeader name="Send Amount">Send Amount</DataTableColumnHeader>
              </DataTableRow>
            </TableHead>

            <TableBody>
              {[...data.request0].reverse().map((c) => (
                <DataTableRow key={c.id}>
                  <DataTableCell> {c.commodity} </DataTableCell>
                  <DataTableCell>{c.date}</DataTableCell>
                  <DataTableCell>{c.time}</DataTableCell>
                  <DataTableCell>{c.hospital}</DataTableCell>
                  <DataTableCell>{c.recipient}</DataTableCell>
                  <DataTableCell>{c.sendAmount}</DataTableCell>
                </DataTableRow>
              ))}
            </TableBody>
          </DataTable>
        </div>
      </>
    );
  }
}
