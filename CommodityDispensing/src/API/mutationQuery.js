export function sendRecount() {
    return {
      resource: "dataValueSets",
      type: "create",
      dataSet: "ULowA8V3ucd",
      data: ({ value, dataElement, period, orgUnit }) => ({
        dataValues: [
          {
            categoryOptionCombo: "rQLFnNXXIL0",
            dataElement: dataElement,
            period: period,
            orgUnit: orgUnit,
            value: parseInt(value),
          },
        ],
      }),
  };
}

export function sendRecountLog() {
    return {
      resource: "dataStore/IN5320-24/recount_log",
      type: "update",
      data: ({ logList }) => logList,
    }
};

export function sendDispensLog() {
    return {
      resource: "dataStore/IN5320-24/dispensing_log",
      type: "update",
      data: ({ logList }) => logList,
    }
};

export function sendDispens() {
    return {
      resource: "dataValueSets",
      type: "create",
      dataSet: "ULowA8V3ucd",
      data: ({ commoditiesMutation }) => ({
        dataValues: commoditiesMutation,
      }),
    }
};