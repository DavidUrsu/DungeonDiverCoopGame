import os

# Define the directory path
directory = r"D:\unity_proitecte\DungeonDiverCoopGame\Assets\Resources\Tiles\Spawn"

# Define the ranges for renaming
ranges = {
    "wall": (0, 13),
    "floor": (14, 34),
    "top": (35, 41)
}

# Iterate through files in the directory
for filename in os.listdir(directory):
    # Check if it's a file
    if os.path.isfile(os.path.join(directory, filename)):
        # Split the filename and extension
        name, ext = filename.split(".")[0], filename.split(".")[1:]
        print(name, ext)
        
        # Extract the number from the filename
        number = int(name.split("_")[-1])
        
        # Determine the category based on the number
        category = None
        for key, (start, end) in ranges.items():
            if start <= number <= end:
                category = key
                break
        
        # If category found, construct new filename and rename
        if category:
            new_filename = f"{category}_{number:02d}.{'.'.join(ext)}"
            print(f"Renaming {filename} to {new_filename}")
            os.rename(os.path.join(directory, filename), os.path.join(directory, new_filename))
