import React, { useState } from "react";
import "../styles.css";
import {
  DropdownButton,
  FlyoutMenu,
  MenuItem,
  DataTable,
  DataTableCell,
  DataTableColumnHeader,
  DataTableRow,
  TableHead,
  TableBody,
} from "@dhis2/ui";

function categoryList(data) {
  let list = [];
  data.map((d) => {
    list.push(d.category);
  });
  // Removing duplicates
  let dup = [...new Set(list)];
  return dup;
}

function editData(data, search, filter) {
  let dataList = data;
  if (search != "") {
    let searchByCommodity = data.filter(({ name }) =>
      name.toLowerCase().includes(search.toLowerCase())
    );
    dataList = searchByCommodity;
  }
  if (filter != "") {
    let filterByCategory = dataList.filter(({ category }) =>
      category.toLowerCase().includes(filter.toLowerCase())
    );
    dataList = filterByCategory;
  }
  return dataList;
}

let apiData = [];
let categoryData = [];

export default function Table(props) {
  if (props.data == "") {
    return <></>;
  }

  const [filterCategory, setFilterCategory] = useState("");
  const [searchCommodity, setSearchCommodity] = useState("");
  const [closeMenu, setcloseMenu] = useState(false);

  function menuClick(c){
    setFilterCategory(c)
    setcloseMenu(false)
  }

  function dropDownMainclick(){
    if(closeMenu == false){
      setcloseMenu(true)
    }
    else {
      setcloseMenu(false)
    }
  }

  apiData = editData(props.data, searchCommodity, filterCategory);
  categoryData = categoryList(props.data);
  props.data.sort((a, b) => (a.name > b.name ? 1 : -1));

  let keys = ""
  let ut = "open"
  // If there is apiData, then finding the keys 
  if (apiData.length > 0){
  keys = Object.keys(apiData[0]);
  }

 if (keys.length == 4)
 {
            return (

              <div>
                <div className="search">
                  <label htmlFor="searchCommodity">Search by commodity</label>
                  <input
                    type="text"
                    name="searchCommodity"
                    id="searchCommodity"
                    placeholder="Search.."
                    onChange={(e) => setSearchCommodity(e.target.value)}
                  />
                </div>
                <div className="filter">
                  <label htmlFor="filterCategory">Filter by category</label>
                  <DropdownButton
                   onClick={() => dropDownMainclick("")}
                   open={closeMenu}
                    className="filterCategory"
                    name="filterCategory"
                    component={
                      <FlyoutMenu>
                        <MenuItem dense
                          label="All Categories"
                          onClick={() => menuClick("")}
                        />
                        {categoryData.map((c) => (
                          <MenuItem dense
                            key={c}
                            label={c}
                            onClick={() => menuClick(c)}
                          />
                        ))}
                      </FlyoutMenu>
                    }
                  >
                    {filterCategory == "" ? "All Categories" : filterCategory}
                  </DropdownButton>
                </div>
                <DataTable className="datatable">
                  <TableHead>
                    <DataTableRow>
                      <DataTableColumnHeader
                        name="name">
                        Name
                      </DataTableColumnHeader>
                      <DataTableColumnHeader
                        name="category">
                        Category
                      </DataTableColumnHeader>
                      <DataTableColumnHeader
                        name="stock">
                        Stock
                      </DataTableColumnHeader>
                    </DataTableRow>
                  </TableHead>
                  <TableBody>
                    {apiData.map((c) => (
                      <DataTableRow key={c.id}>
                        <DataTableCell> {c.name} </DataTableCell>
                        <DataTableCell>{c.category}</DataTableCell>
                        <DataTableCell>{c.endBalance}</DataTableCell>
                      </DataTableRow>
                    ))}
                  </TableBody>
                </DataTable>
              </div>
            );
    }
 else {
  return (
    <div>
      <div className="search">
        <label htmlFor="searchCommodity">Search by commodity</label>
        <input
          type="text"
          name="searchCommodity"
          id="searchCommodity"
          placeholder="Search.."
          onChange={(e) => setSearchCommodity(e.target.value)}
        />
      </div>
      <div className="filter">
        <label htmlFor="filterCategory">Filter by category</label>
        <DropdownButton
        onClick={() => dropDownMainclick("")}
          open={closeMenu}
          className="filterCategory"
          name="filterCategory"
          component={
            <FlyoutMenu>
              <MenuItem
                dense
                label="All Categories"
                onClick={() => menuClick("")}
              />
              {categoryData.map((c) => (
                <MenuItem
                  dense
                  key={c}
                  label={c}
                  onClick={() =>  menuClick(c) }
                />
              ))}
            </FlyoutMenu>
          }
        >
          {filterCategory == "" ? "All Categories" : filterCategory}
        </DropdownButton>
      </div>
      <DataTable className="datatable">
        <TableHead>
          <DataTableRow >
            
            <DataTableColumnHeader
              name="name">
              Name
            </DataTableColumnHeader>
            <DataTableColumnHeader
              name="category">
              Category
            </DataTableColumnHeader>
            <DataTableColumnHeader
              name="consumption">
              Consumption
            </DataTableColumnHeader>
            <DataTableColumnHeader
              name="toOrder">
              To be ordered
            </DataTableColumnHeader>
            <DataTableColumnHeader
              name="stock">
              Stock
            </DataTableColumnHeader>
          </DataTableRow>
        </TableHead>

        <TableBody>
          {apiData.map((c) => (
            <DataTableRow key={c.id}>
              <DataTableCell> {c.name} </DataTableCell>
              <DataTableCell>{c.category}</DataTableCell>
              <DataTableCell>{c.consumption}</DataTableCell>
              <DataTableCell>{c.toOrder}</DataTableCell>
              <DataTableCell>{c.endBalance}</DataTableCell>
            </DataTableRow>
          ))}
        </TableBody>
      </DataTable>
    </div>
  );
 }
}
