import { useFormik } from "formik";
import React from "react";
import { CreateContentQuery } from "../../../app/models/content";
import { useStore } from "../../../app/stores/store";


export default function ImportModal() {
    const { contentStore: { importContent } } = useStore();
    const formik = useFormik<CreateContentQuery>({
        initialValues: {
            contentUrl: '',
            description: '',
            tags: []
        },
        onSubmit(values, helpers) {
            importContent(values).then(() => {
                console.log(`Imported content from url: ${values.contentUrl}`);
            })
        }
    }
    );

    return (
        <div className="container">
            <form
                onSubmit={formik.handleSubmit}>
                <label htmlFor="contentUrl">Source URL:</label>
                <input
                    name="contentUrl"
                    id="contentUrl"
                    type="text"
                    onChange={formik.handleChange}
                    value={formik.values.contentUrl}
                />
                <label htmlFor="description">Description:</label>
                <input
                    name="description"
                    id="description"
                    type="text"
                    onChange={formik.handleChange}
                    value={formik.values.description}
                />
                <button
                className="btn bg-dark text-light" 
                type="submit">Import</button>
            </form>
        </div>
    )

}