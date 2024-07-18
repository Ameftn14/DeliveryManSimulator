using UnityEngine;
using System.Collections.Generic;

public class ColorDictionary {
    private static readonly Dictionary<int, ColorValue> colorDictionary = new()
    {
        {0, new ColorValue(0.039f, 0.518f, 1f)},     // 蓝色
        {1, new ColorValue(1f, 0.839f, 0.039f)},     // 黄色
        {2, new ColorValue(0.75f, 0.353f, 0.95f)},   // 紫色
        {3, new ColorValue(1f, 0.624f, 0.039f)},     // 橙色
        {4, new ColorValue(0.196f, 0.843f, 0.294f)}, // 绿色
        {5, new ColorValue(1f, 0f, 1f)},             // 品红
        {6, new ColorValue(0.5f, 0.7f, 0.039f)},       // 橙黄色
        {7, new ColorValue(1f, 0.5f, 0.5f)},         // 粉红
        {8, new ColorValue(0f, 0.7f, 0.7f)},         // 金色
        {9, new ColorValue(0.68f, 0.56f, 0.17f)},          // 栗色
        {10, new ColorValue(0.57f, 0.99f, 0.95f)}         // 橄榄色
    };

    public static int GetColorIndex(int index) {
        if (colorDictionary.Count == 0) {
            Debug.LogError("Color dictionary is empty!");
            return -1;
        }

        // 取模运算确保索引在有效范围内循环
        int modIndex = Mathf.Abs(index % colorDictionary.Count);

        while(colorDictionary[modIndex].Busy) {
            modIndex++;
            modIndex %= colorDictionary.Count;
        }
        colorDictionary[modIndex].Busy = true;

        // 使用取模后的索引获取颜色
        return modIndex;
    }

    public static Color GetColor(int index) {
        return colorDictionary[GetColorIndex(index)].Color;
    }

    public static Color PeekColor(int index) {
        return colorDictionary[index].Color;
    }

    // public static ColorValue GetColorValue(int index) {
    //     return new ColorValue(GetColor(index));
    //}
    public static void ReleaseColor(int index) {
        int modIndex = Mathf.Abs(index % colorDictionary.Count);
        colorDictionary[modIndex].Busy = false;
    }
}

public class ColorValue {
    public bool Busy { get; set; }
    public Color Color { get; set; }

    public ColorValue(Color color) {
        Busy = false;
        Color = color;
    }

    public ColorValue(float r, float g, float b) {
        Busy = false;
        Color = new Color(r, g, b);
    }
}
