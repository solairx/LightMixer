import argparse
import musicnn
import numpy as np
import matplotlib.pyplot as plt
import matplotlib.rcsetup as rcsetup
import matplotlib
import json
from json import JSONEncoder
from musicnn.extractor  import extractor
from musicnn.tagger import top_tags
import logging
import warnings
import sys

warnings.filterwarnings("ignore", category=UserWarning)

logging.basicConfig(level=logging.CRITICAL)

class NumpyArrayEncoder(JSONEncoder):
    def default(self, obj):
        if isinstance(obj, np.ndarray):
            return obj.tolist()
        return JSONEncoder.default(self, obj)

taggramSmall, tagsSmall = extractor(sys.argv[1], model='MSD_musicnn_big', input_length=3, input_overlap=0.5, extract_features=False)
taggram, tags = extractor(sys.argv[1], model='MSD_musicnn_big', input_length=5, input_overlap=False, extract_features=False)
json_str = "{\n \"Version\":\"1.0\",\n\"SmallSampleTags\":" + json.dumps(tagsSmall, indent=4,cls=NumpyArrayEncoder)+ ",\n\"SmallSampleRate\":"+ json.dumps(taggramSmall, indent=4,cls=NumpyArrayEncoder) +",\n\"BigSampleTags\":"+ json.dumps(tags, indent=4,cls=NumpyArrayEncoder)+ ",\n\"BigSampleRate\":"+ json.dumps(taggram, indent=4,cls=NumpyArrayEncoder) +"\n}"
print ("Result = ")
print (json_str)


in_length = 3 # seconds  by default, the model takes inputs of 3 seconds with no overlap

plt.rcParams["figure.figsize"] = (10,8) # set size of the figures
fontsize = 12 # set figures font size
fig, ax = plt.subplots()

# title
ax.title.set_text('Taggram')
ax.title.set_fontsize(fontsize)

# x-axis title
ax.set_xlabel('(seconds)', fontsize=fontsize)

# y-axis
y_pos = np.arange(len(tags))
ax.set_yticks(y_pos)
ax.set_yticklabels(tags, fontsize=fontsize-1)

# x-axis
x_pos = np.arange(taggram.shape[0])
x_label = np.arange(in_length/2, in_length*taggram.shape[0], 3)
ax.set_xticks(x_pos)
ax.set_xticklabels(x_label, fontsize=fontsize)

# depict taggram
ax.imshow(taggram.T, interpolation=None, aspect="auto")
plt.savefig("test1.png")
plt.show()