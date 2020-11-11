using UnityEngine;
using UnityEngine.UI;

public class MC_SettingsPanel : MonoBehaviour
{
    public NormalTreeGenerationData normalTreeGenData = new NormalTreeGenerationData { widthRange = new Vector2Int(4, 4), heightRange = new Vector2Int(5, 5)
        , branchesAmountRange = new Vector2Int(2, 2), childLevels = 1, nestedTreeAmountRange = new Vector2Int(1, 1) };

    public static MC_SettingsPanel instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetNormalTreeWidthRange(Text text)
    {
        int width;
        if (int.TryParse(text.text, out width))
            normalTreeGenData.widthRange = new Vector2Int(width, width);
    }

    public void SetNormalTreeHeightRange(Text text)
    {
        int height;
        if (int.TryParse(text.text, out height))
            normalTreeGenData.heightRange = new Vector2Int(height, height);
    }

    public void SetNormalTreeBranchesAmountRange(Text text)
    {
        int branches;
        if (int.TryParse(text.text, out branches))
            normalTreeGenData.branchesAmountRange = new Vector2Int(branches, branches);
    }

    public void SetNormalTreeLevels(Text text)
    {
        int levels;
        if (int.TryParse(text.text, out levels))
            normalTreeGenData.childLevels = levels;
    }

    public void SetNormalTreeNestedTreesAmount(Text text)
    {
        int nestedTrees;
        if (int.TryParse(text.text, out nestedTrees))
            normalTreeGenData.nestedTreeAmountRange = new Vector2Int(nestedTrees, nestedTrees);
    }
}
