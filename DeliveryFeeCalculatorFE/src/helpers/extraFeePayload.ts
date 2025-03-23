export function buildExtraFeePayload(
    id: string,
    formValues: {
      conditionType: string;
      feeAmount: string;
      minValue: string;
      maxValue: string;
      vehicleTypeId: string;
    }
  ) {
    return {
      id,
      vehicleTypeId: formValues.vehicleTypeId,
      conditionType: formValues.conditionType,
      feeAmount: parseFloat(formValues.feeAmount),
      minValue: formValues.minValue === "" ? null : parseFloat(formValues.minValue),
      maxValue: formValues.maxValue === "" ? null : parseFloat(formValues.maxValue),
    };
  }