export function mergeExternalData(data) {
    return data.dataSets.dataSetElements.map((d) => {
        let categoryName;
        d.dataElement.dataElementGroups.map((category) => {
          if (category.id !== "Svac1cNQhRS") {
            categoryName = category.name;
          }
        });
        const categoryValue = d.dataElement.categoryCombo.categoryOptionCombos.map(
          (v) => {
            const matchedValue = data.dataValueSets.dataValues.find((dataValue) => {
              if (dataValue.dataElement == d.dataElement.id) {
                if (dataValue.categoryOptionCombo == v.id) {
                  return true;
                }
              }
            });
            if (matchedValue) {
              return {
                name: v.name,
                value: matchedValue.value,
              };
            }
          }
        );
        if (categoryValue) {
          return {
            name: d.dataElement.name.substring(14, d.dataElement.name.length),
            id: d.dataElement.id,
            category: categoryName.substring(12, categoryName.length),
            endBalance: categoryValue[2].value,
          };
        }
      });
}

export function mergeOverviewData(data) {
  return data.dataSets.dataSetElements.map((d) => {
    let categoryName;
    d.dataElement.dataElementGroups.map((category) => {
      if (category.id !== "Svac1cNQhRS") {
        categoryName = category.name;
      }
    });

    const categoryValue = d.dataElement.categoryCombo.categoryOptionCombos.map(
      (v) => {
        const matchedValue = data.dataValueSets.dataValues.find((dataValue) => {
          if (dataValue.dataElement == d.dataElement.id) {
            if (dataValue.categoryOptionCombo == v.id) {
              return true;
            }
          }
        });
        return {
          name: v.name,
          value: matchedValue.value,
        };
      }
    );

    const consumption = categoryValue.find(
      (c) => c.name === "Consumption"
    ).value;
    const toOrder = categoryValue.find(
      (c) => c.name === "Quantity to be ordered"
    ).value;
    const endBalance = categoryValue.find(
      (c) => c.name === "End Balance"
    ).value;

    return {
      name: d.dataElement.name.substring(14, d.dataElement.name.length),
      id: d.dataElement.id,
      category: categoryName.substring(12, categoryName.length),
      consumption: consumption,
      toOrder: toOrder,
      endBalance: endBalance,
    };
  });
}

export function mergeRecountingData(data) {
  return data.dataSets.dataSetElements.map((d) => {
    let categoryName;
    d.dataElement.dataElementGroups.map((category) => {
      if (category.id !== "Svac1cNQhRS") {
        categoryName = category.name;
      }
    });

    const categoryValue = d.dataElement.categoryCombo.categoryOptionCombos.map(
      (v) => {
        const matchedValue = data.dataValueSets.dataValues.find((dataValue) => {
          if (dataValue.dataElement == d.dataElement.id) {
            if (dataValue.categoryOptionCombo == v.id) {
              return true;
            }
          }
        });
        return {
          name: v.name,
          value: matchedValue.value,
        };
      }
    );

    const endBalance = categoryValue.find(
      (c) => c.name === "End Balance"
    ).value;

    return {
      name: d.dataElement.name,
      id: d.dataElement.id,
      category: categoryName,
      endBalance: endBalance,
    };
  });
}

export function mergeDispensData(data) {
  return data.dataSets.dataSetElements.map((d) => {
    let matchedVal = data.dataValueSets.dataValues.find((dataValues) => {
      if (dataValues.dataElement == d.dataElement.id) return true;
    });
    return {
      name: d.dataElement.name,
      id: d.dataElement.id,
      value: matchedVal.value,
      orgUnit: matchedVal.orgUnit,
    };
  });
}
