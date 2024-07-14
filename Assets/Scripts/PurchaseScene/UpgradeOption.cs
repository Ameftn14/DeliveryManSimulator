public enum UpgradeType {

    PermanentSpeedBoost,
    BiggerStorage,
    TempararySpeedBoost,
    TempararyTimeSlow
};

public class UpgradeOption {
    public readonly UpgradeType type;
    public readonly bool isAvailable;
    public UpgradeOption(UpgradeType type, bool isAvailable = true) {
        this.type = type;
        this.isAvailable = isAvailable;
    }
}