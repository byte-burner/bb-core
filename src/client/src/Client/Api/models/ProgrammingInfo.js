export class ProgrammingInfo {
  constructor({
    DeviceType, BridgeType, BridgeSerialNbr, ProgramFilePath,
  }) {
    this.deviceType = DeviceType;
    this.bridgeType = BridgeType;
    this.bridgeSerialNbr = BridgeSerialNbr;
    this.programFilePath = ProgramFilePath;
  }
}
