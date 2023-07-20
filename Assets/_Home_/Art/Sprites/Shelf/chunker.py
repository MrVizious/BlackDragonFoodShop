import os
from PIL import Image

# Get the current directory
current_directory = os.getcwd()

# Iterate through all files in the directory
for file_name in os.listdir(current_directory):
    if file_name.endswith("estanteria_sprite.png"):
        # Open the image file
        image_path = os.path.join(current_directory, file_name)
        image = Image.open(image_path)

        # Get the width and height of the image
        width, height = image.size

        # Calculate the new dimensions including padding and separation
        chunk_size = 16  # Chunk size of 48x48 pixels
        padding_horizontal = 2  # Horizontal padding of 30 pixels
        padding_vertical = 2  # Vertical padding of 10 pixels
        separation_horizontal = 0  # Horizontal separation of 20 pixels
        separation_vertical = 2  # Vertical separation of 5 pixels
        new_width = (chunk_size + separation_horizontal) * \
            (width // chunk_size) + 2 * padding_horizontal
        new_height = (chunk_size + separation_vertical) * \
            (height // chunk_size) + 2 * padding_vertical

        # Create a new image with transparent background
        new_image = Image.new("RGBA", (new_width, new_height), (0, 0, 0, 0))

        # Iterate through each chunk of the original image
        for y in range(0, height, chunk_size):
            for x in range(0, width, chunk_size):
                # Extract the chunk from the original image
                chunk = image.crop((x, y, x + chunk_size, y + chunk_size))

                # Calculate the position of the chunk in the new image
                new_x = padding_horizontal + x // chunk_size * \
                    (chunk_size + separation_horizontal)
                new_y = padding_vertical + y // chunk_size * \
                    (chunk_size + separation_vertical)

                # Paste the chunk onto the new image without resizing
                new_image.paste(chunk, (new_x, new_y))

        # Save the modified image, overwriting the original file
        new_image.save(image_path)
