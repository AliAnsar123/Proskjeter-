import React from "react";
import { useDataQuery } from "@dhis2/app-runtime";
import { recountDataStoreQuery } from "../API/dataQuery";
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


export function RecountingLog() {
  const { loading, error, data } = useDataQuery(recountDataStoreQuery());

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
                <DataTableColumnHeader name="Name">Name</DataTableColumnHeader>
                <DataTableColumnHeader name="Category">
                  Category
                </DataTableColumnHeader>
                <DataTableColumnHeader name="Date">Date</DataTableColumnHeader>
                <DataTableColumnHeader name="Time">Time</DataTableColumnHeader>
                <DataTableColumnHeader name="OldBalance">
                  Old Balance
                </DataTableColumnHeader>
                <DataTableColumnHeader name="NewBalance">
                  New Balance
                </DataTableColumnHeader>
              </DataTableRow>
            </TableHead>

            <TableBody>
              {[...data.request0].reverse().map((c) => (
                <DataTableRow key={c.id}>
                  <DataTableCell> {c.name} </DataTableCell>
                  <DataTableCell>{c.category}</DataTableCell>
                  <DataTableCell>{c.date}</DataTableCell>
                  <DataTableCell>{c.time}</DataTableCell>
                  <DataTableCell>{c.oldValue}</DataTableCell>
                  <DataTableCell>{c.newValue}</DataTableCell>
                </DataTableRow>
              ))}
            </TableBody>
          </DataTable>
        </div>
      </>
    );
  }
}
