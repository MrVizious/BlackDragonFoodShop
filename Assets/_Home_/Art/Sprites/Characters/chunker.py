import os
from PIL import Image

# Get the current directory
current_directory = os.getcwd()

# Iterate through all files in the directory
for file_name in os.listdir(current_directory):
    if file_name.endswith(".png"):
        # Open the image file
        image_path = os.path.join(current_directory, file_name)
        image = Image.open(image_path)

        # Get the width and height of the image
        width, height = image.size

        # Calculate the new dimensions including padding and separation
        chunk_size = 48  # Chunk size of 48x48 pixels
        padding = 20  # Padding of 20 pixels
        separation = 20  # Separation of 20 pixels
        new_width = (chunk_size + separation) * (width // chunk_size) + 2 * padding
        new_height = (chunk_size + separation) * (height // chunk_size) + 2 * padding

        # Create a new image with transparent background
        new_image = Image.new("RGBA", (new_width, new_height), (0, 0, 0, 0))

        # Iterate through each chunk of the original image
        for y in range(0, height, chunk_size):
            for x in range(0, width, chunk_size):
                # Extract the chunk from the original image
                chunk = image.crop((x, y, x + chunk_size, y + chunk_size))

                # Calculate the position of the chunk in the new image
                new_x = padding + x // chunk_size * (chunk_size + separation)
                new_y = padding + y // chunk_size * (chunk_size + separation)

                # Paste the chunk onto the new image without resizing
                new_image.paste(chunk, (new_x, new_y))

        # Save the modified image, overwriting the original file
        new_image.save(image_path)