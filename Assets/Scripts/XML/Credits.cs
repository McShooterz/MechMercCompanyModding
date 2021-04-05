using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Credits
{
    public CreditsGroup[] CreditsGroups = new CreditsGroup[0];

    public string GetDisplay()
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

        foreach (CreditsGroup creditsGroup in CreditsGroups)
        {
            stringBuilder.Append(creditsGroup.GetDisplay());
        }

        return stringBuilder.ToString();
    }

    public class CreditsGroup
    {
        public string Title = "";

        public string[] Elements = new string[0];

        public int Spacing = 5;

        public string GetDisplay()
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

            stringBuilder.AppendLine(Title);

            stringBuilder.AppendLine();

            foreach (string element in Elements)
            {
                stringBuilder.AppendLine(element);
            }

            for (int i = 0; i < Spacing; i++)
            {
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}
