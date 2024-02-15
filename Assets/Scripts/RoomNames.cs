using System;
using System.Collections.Generic;

class RoomNames {
    private string[] normalSections = {
        "Living Room",
        "Bedroom",
        "Kitchen",
        "Dining Room",
        "Bathroom",
        "Home Office",
        "Children's Room",
        "Storage Solutions",
        "Outdoor Furniture",
        "Small Spaces",
        "Hallway",
        "Family Section",
        "Business Section",
        "Self-Serve Warehouse",
        "Lighting",
        "Textiles",
        "Rugs",
        "Decorations",
        "Marketplace",
        "Study",
        "Patio",
        "Entertainment",
        "Garden",
        "Workspace",
        "Laundry Room",
        "Chairs",
        "Plants",
        "Tableware",
        "Returns",
        "Warehouse"
    };

    private string[] abnormalSections = {
        "Loading Dock",
        "Vintage Doorknobs",
        "Sock Puppets",
        "Trash Compactor",
        "Industrial Cleaning Supplies",
        "Rusty Keys",
        "Umbrellas",
        "Expired Coupons",
        "Spare Parts",
        "Unused Cables",
        "Skin Flakes",
        "Junk Mail",
        "Employee Processing",
        "Lost and Found",
    };

    public string GetRandomName()
    {
        int randomNumber = UnityEngine.Random.Range(0, 100);
        if (randomNumber < 90) {
            return GetRandomElementFromArray(normalSections);
        } else {
            return GetRandomElementFromArray(abnormalSections);
        }
    }
    private string GetRandomElementFromArray(string[] array) {
        int randomIndex = UnityEngine.Random.Range(0, array.Length);
        return array[randomIndex];
    }
}