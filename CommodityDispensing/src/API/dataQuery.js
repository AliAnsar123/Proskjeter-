export function dataQuery() {
  return {
    dataSets: {
      resource: "dataSets/ULowA8V3ucd",
      params: {
        fields: [
          "dataSetElements[dataElement[name,id,categoryCombo[categoryOptionCombos[name,id]],dataElementGroups[name,id]]]",
        ],
      },
    },
    dataValueSets: {
      resource: "dataValueSets",
      params: {
        orgUnit: "uYG1rUdsJJi",
        dataSet: "ULowA8V3ucd",
        period: "202110",
      },
    },
  };
}

export function dispensingDataQuery() {
  return {
  dataSets: {
    resource: "dataSets/ULowA8V3ucd",
    params: {
      fields: ["name,id,dataSetElements[dataElement[name,id,orgUnit]]"],
    },
  },
  dataValueSets: {
    resource: "dataValueSets",
    params: {
      orgUnit: "uYG1rUdsJJi",
      dataSet: "ULowA8V3ucd",
      period: "202110",
    },
  },
  dataStore: {
    resource: "/dataStore/IN5320-24/dispensing_log",
  },
}
};

export function recountQuery() {
  return {
    dataSets: {
      resource: "dataSets/ULowA8V3ucd",
      params: {
        fields: [
          "dataSetElements[dataElement[name,id,categoryCombo[categoryOptionCombos[name,id]],dataElementGroups[name,id]]]",
        ],
      },
    },
    dataValueSets: {
      resource: "dataValueSets",
      params: {
        orgUnit: "uYG1rUdsJJi",
        dataSet: "ULowA8V3ucd",
        period: "202110",
      },
    },
    dataStore: {
      resource: "/dataStore/IN5320-24/recount_log",
    },
  };
}

export function orgQuery(){
  return ({
      orgSet: {
          resource: "/organisationUnits",
        },
    dataSets: {
    resource: "dataSets/ULowA8V3ucd",
    params: {
      fields:  "dataSetElements[dataElement[name,id,categoryCombo[categoryOptionCombos[name,id]],dataElementGroups[name,id]]]",
    },
  },
  dataValueSets: {
    resource: "dataValueSets",
    params: ({ orgUnit }) => ({
      orgUnit,
      dataSet: "ULowA8V3ucd",
      period: "202110",
    }),
  },
})
}

export function recountDataStoreQuery(){
  return ({
  request0: {
    resource: "/dataStore/IN5320-24/recount_log",
  },
})
};

export function dispensDataStoreQuery(){
  return ({
  request0: {
    resource: "/dataStore/IN5320-24/dispensing_log",
  },
})
};

