public enum UpgradeType {
    PermanentSpeedBoost,
    BiggerStorage,
    TempararySpeedBoost,
    TempararyTimeSlow
};

public class UpgradeOption {
    public  UpgradeType type;
    public  bool isAvailable;
    public UpgradeOption(UpgradeType type, bool isAvailable = true) {
        this.type = type;
        this.isAvailable = isAvailable;
    }


}